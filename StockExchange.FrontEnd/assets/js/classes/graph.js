import { getProperty } from "../utils/styleUtils.js";
import { Company } from "./company.js";

export class GraphConstants {
    static maxGraphNodes = 30;
}

class GraphPoint {
    #x;
    #y;

    constructor(x, y) {
        this.#x = x;
        this.#y = y;
    }

    get X() {
        return this.#x;
    }

    get Y() {
        return this.#y;
    }
}

class GraphLine {
    #startPoint;
    #endPoint;

    constructor(startPoint, endPoint) {
        this.#startPoint = startPoint;
        this.#endPoint = endPoint;
    }

    get StartPoint() {
        return this.#startPoint;
    }

    get EndPoint() {
        return this.#endPoint;
    }
}

export class Graph {
    #svg;
    #scale;

    /**
     * @param {SVGElement} graphElement 
     */
    constructor(graphElement) {
        this.#svg = graphElement;
    }

    /**
     * 
     * @param {GraphLine} graphLine 
     * @returns 
     */
    createLine(graphLine, lineWidth) {
        const newHoverGroup = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        const newHoverRect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        const newCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
        const newLine = document.createElementNS('http://www.w3.org/2000/svg', 'line');

        newHoverRect.setAttribute('stroke', 'none');
        newHoverRect.setAttribute('fill', 'none');
        newHoverRect.setAttribute('height', '100%');
        newHoverRect.setAttribute('width', graphLine.EndPoint.X - graphLine.StartPoint.X);
        newHoverRect.setAttribute('x', graphLine.StartPoint.X);
        newHoverRect.setAttribute('y', 0);

        newLine.setAttribute('stroke-width', lineWidth);
        newLine.setAttribute('x1', graphLine.StartPoint.X);
        newLine.setAttribute('x2', graphLine.EndPoint.X);
        newLine.setAttribute('y1', graphLine.StartPoint.Y);
        newLine.setAttribute('y2', graphLine.EndPoint.Y);

        newCircle.setAttribute('fill', 'none');
        newCircle.setAttribute('stroke', 'none');
        newCircle.setAttribute('stroke-width', lineWidth);
        newCircle.setAttribute('cx', graphLine.EndPoint.X);
        newCircle.setAttribute('cy', graphLine.EndPoint.Y);
        newCircle.setAttribute('r', lineWidth * Math.PI);

        newHoverGroup.appendChild(newHoverRect);
        newHoverGroup.appendChild(newLine);
        newHoverGroup.appendChild(newCircle);

        return newHoverGroup;
    }

    /**
     * 
     * @param {Company} company 
     */
    updateGraph(company, daysToShow = 1) {
        let filteredCompanyData = company.getDataForDays(daysToShow);
        let yMax, xMax;

        if (filteredCompanyData.length > GraphConstants.maxGraphNodes) {
            filteredCompanyData = company.getAverageDataInChunks(daysToShow);
        }
        
        yMax = Math.max(...filteredCompanyData.map(data => data.Price));
        xMax = filteredCompanyData.length - 1;

        console.info(filteredCompanyData);

        // scope SVG to max values
        this.#svg.setAttribute('viewBox', `0 0 ${xMax} ${yMax}`);
        this.#scale = 0.05;

        // Set fill style
        const valueIncreased = filteredCompanyData[0].Price < filteredCompanyData[filteredCompanyData.length - 1].Price;
        this.#svg.classList.toggle('stock-exchange--positive', valueIncreased);
        this.#svg.classList.toggle('stock-exchange--negative', !valueIncreased);

        // Clear canvas
        this.#svg.innerHTML = '';

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

        // Draw a dashed line through the horizontal center
        const dashedCenterLine = document.createElementNS('http://www.w3.org/2000/svg','line');
        dashedCenterLine.setAttribute('stroke-width', this.#scale * 0.75);
        dashedCenterLine.setAttribute('stroke-dasharray', '0.1');
        dashedCenterLine.setAttribute('x1', 0);
        dashedCenterLine.setAttribute('x2', filteredCompanyData.length - 1);
        dashedCenterLine.setAttribute('y1', yMax - filteredCompanyData[0].Price);
        dashedCenterLine.setAttribute('y2', yMax - filteredCompanyData[0].Price);
        dashedCenterLine.classList.add('stock-exchange__graph-center-line');
        this.#svg.appendChild(dashedCenterLine);

        // Draw line chart of company value over time
        let previousPoint = new GraphPoint(0, yMax - filteredCompanyData[0].Price);
        [...filteredCompanyData].forEach((data, index) => {
            const newGraphPoint = new GraphPoint(index, yMax - data.Price);
            const newGraphLine = new GraphLine(previousPoint, newGraphPoint);
            const newLine = this.createLine(newGraphLine, this.#scale);
            previousPoint = newGraphPoint;
            this.#svg.appendChild(newLine);
        });
    }
}