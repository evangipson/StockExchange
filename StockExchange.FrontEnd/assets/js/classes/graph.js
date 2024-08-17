import { getProperty } from "../utils/styleUtils.js";
import { Company } from "./company.js";

export class GraphConstants {
    static maxGraphNodes = 30;
}

export class Graph {
    #canvas;
    #canvasContext;
    #xScale;
    #yScale;

    /**
     * @param {HTMLElement} canvasElement 
     */
    constructor(canvasElement) {
        this.#canvas = canvasElement;
        this.#canvas.height = canvasElement.clientHeight;
        this.#canvas.width = canvasElement.clientWidth;
        this.#canvasContext = canvasElement.getContext('2d');
        window.addEventListener('resize', () => resizeGraph(canvasElement), false);
    }

    resizeGraph(canvasElement) {
        this.#canvas.height = canvasElement.clientHeight;
        this.#canvas.width = canvasElement.clientWidth;
    }

    /**
     * 
     * @param {Company} company 
     */
    updateGraph(company, daysToShow = 1) {
        let filteredCompanyData = company.getDataForDays(daysToShow);
        const xMin = 0;
        const yMin = 0;
        let yMax, xMax;

        if (filteredCompanyData.length > GraphConstants.maxGraphNodes) {
            filteredCompanyData = company.getAverageDataInChunks(daysToShow);
        }
        
        yMax = filteredCompanyData.toSorted((a, b) => b.Price - a.Price)[0].Price;
        xMax = filteredCompanyData.length - 1;
        this.#xScale = (this.#canvas.width / (xMax - xMin));
        this.#yScale = (this.#canvas.height / (yMax - yMin));

        this.#canvasContext.scale(this.#xScale, this.#yScale);

        // Set fill style
        this.#canvasContext.strokeStyle = getProperty('color-negative');
        this.#canvasContext.fillStyle = getProperty('color-negative-transparent');
        if (filteredCompanyData[0].Price < filteredCompanyData[filteredCompanyData.length - 1].Price) {
            this.#canvasContext.strokeStyle = getProperty('color-positive');
            this.#canvasContext.fillStyle = getProperty('color-positive-transparent');
        }
        this.#canvasContext.lineWidth = 1 / this.#xScale;

        // Clear canvas
        this.#canvasContext.clearRect(0, 0, xMax, yMax);

        // Draw x-axis
        //this.#canvasContext.beginPath();
        //this.#canvasContext.moveTo(0, yMax - yMin);
        //this.#canvasContext.lineTo(xMax, yMax - yMin);
        //this.#canvasContext.stroke();

        // Draw y-axis
        //this.#canvasContext.beginPath();
        //this.#canvasContext.moveTo(0, 0);
        //this.#canvasContext.lineTo(0, yMax - yMin);
        //this.#canvasContext.stroke();

        // Draw line of graph
        this.#canvasContext.beginPath();
        this.#canvasContext.moveTo(0, yMax - filteredCompanyData[0].Price);
        [...filteredCompanyData].forEach((data, index) => {
            this.#canvasContext.lineTo(index, yMax - data.Price);
        });
        this.#canvasContext.stroke();

        // Draw "filled" area below graph
        this.#canvasContext.beginPath();
        this.#canvasContext.moveTo(0, this.#canvas.height - filteredCompanyData[0].Price);
        [...filteredCompanyData].forEach((data, index) => {
            this.#canvasContext.lineTo(index, yMax - data.Price);
        });
        this.#canvasContext.lineTo(filteredCompanyData.length - 1, yMax);
        this.#canvasContext.fill();

        // this.#canvas.addEventListener('mousemove', (event) => {
        //     let rect = this.#canvas.getBoundingClientRect(),
        //         x = event.clientX - rect.left,
        //         y = event.clientY - rect.top;
        //     const nearestPointIndex = [...filteredCompanyData].find((data, index) => {
        //         return index + x >= 1;
        //     });

        //     if(!nearestPointIndex) {
        //         return;
        //     }

        //     if(this.#canvasContext.isPointInPath(x, y)) {
        //         console.info('point in path');
        //         this.#canvasContext.beginPath();
        //         this.#canvasContext.moveTo(x, yMax - nearestPointIndex.Price);
        //         this.#canvasContext.arc(x, yMax - nearestPointIndex.Price, 1, 0, Math.PI * 1);
        //         this.#canvasContext.fill();
        //         this.#canvasContext.closePath();
        //     }
        // });
    }
}