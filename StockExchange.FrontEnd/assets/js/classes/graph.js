import { chunk } from "../utils/collectionUtils.js";
import { Company } from "./company.js";

export class GraphConstants {
    static maxGraphNodes = 60;
}

class GraphPoint {
    #x;
    // y is price
    #y;
    #date;

    constructor(x, y, date) {
        this.#x = x;
        this.#y = y;
        this.#date = date;
    }

    get X() {
        return this.#x;
    }

    get Y() {
        return this.#y;
    }

    get Date() {
        return this.#date;
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
     * Creates an SVG `<circle` element which represents a point on the graph.
     * @param {GraphPoint} graphPoint 
     */
    createCircleForPoint(graphPoint) {
        const newCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');

        newCircle.setAttribute('fill', 'none');
        newCircle.setAttribute('stroke', 'none');
        newCircle.setAttribute('stroke-width', this.#scale * 1.5);
        newCircle.setAttribute('cx', graphPoint.X);
        newCircle.setAttribute('cy', graphPoint.Y);
        newCircle.setAttribute('r', this.#scale * Math.PI);

        return newCircle;
    }

    /**
     * Creates an SVG `<line>` top to bottom wherever the point is.
     * @param {GraphPoint} graphPoint 
     */
    createValueLineForPoint(graphPoint) {
        const newLine = document.createElementNS('http://www.w3.org/2000/svg', 'line');

        newLine.setAttribute('stroke-width', this.#scale);
        newLine.setAttribute('x1', graphPoint.X);
        newLine.setAttribute('x2', graphPoint.X);
        newLine.setAttribute('y1', 0);
        newLine.setAttribute('y2', '100%');
        newLine.classList.add('stock-exchange__graph-value-line');

        return newLine;
    }

    /**
     * Creates an SVG `<rect>` top to bottom around the point, meant
     * for user hover interaction capture.
     * @param {GraphPoint} graphPoint 
     */
    createHoverAreaForPoint(graphPoint) {
        const hoverRect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');

        hoverRect.setAttribute('stroke', 'none');
        hoverRect.setAttribute('fill', 'none');
        hoverRect.setAttribute('height', '100%');
        hoverRect.setAttribute('width', 1);
        hoverRect.setAttribute('x', graphPoint.X - 0.5);
        hoverRect.setAttribute('y', 0);
        hoverRect.classList.add('stock-exchange__graph-value-rect');

        return hoverRect;
    }

    /**
     * Creates an SVG `<line>` between two points.
     * @param {GraphPoint} startPoint 
     * @param {GraphPoint} endPoint 
     * @returns 
     */
    createLineForTwoPoints(startPoint, endPoint) {
        const newLine = document.createElementNS('http://www.w3.org/2000/svg', 'line');

        newLine.setAttribute('stroke-width', this.#scale);
        newLine.setAttribute('x1', startPoint.X);
        newLine.setAttribute('x2', endPoint.X);
        newLine.setAttribute('y1', startPoint.Y);
        newLine.setAttribute('y2', endPoint.Y);

        return newLine;
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

        // scope SVG to max values
        this.#svg.setAttribute('viewBox', `0 -1 ${xMax + 1} ${yMax}`);
        this.#scale = 0.05;

        // Set fill style
        const valueIncreased = filteredCompanyData[0].Price < filteredCompanyData[filteredCompanyData.length - 1].Price;
        this.#svg.classList.toggle('stock-exchange--positive', valueIncreased);
        this.#svg.classList.toggle('stock-exchange--negative', !valueIncreased);

        // Clear canvas
        this.#svg.innerHTML = '';

        // Draw a dashed line through the horizontal center
        const dashedCenterLine = document.createElementNS('http://www.w3.org/2000/svg','line');
        dashedCenterLine.setAttribute('stroke-width', this.#scale * 0.75);
        dashedCenterLine.setAttribute('stroke-dasharray', '0.25');
        dashedCenterLine.setAttribute('x1', 0);
        dashedCenterLine.setAttribute('x2', filteredCompanyData.length - 1);
        dashedCenterLine.setAttribute('y1', yMax - filteredCompanyData[0].Price);
        dashedCenterLine.setAttribute('y2', yMax - filteredCompanyData[0].Price);
        dashedCenterLine.classList.add('stock-exchange__graph-center-line');
        this.#svg.appendChild(dashedCenterLine);

        // Get all points of value in dataset
        const points = [...filteredCompanyData].map((data, index) => new GraphPoint(index, yMax - data.Price, data.Date));

        // Create groups for each collection of points
        const groups = points.map(() => document.createElementNS('http://www.w3.org/2000/svg', 'g'));

        // Draw circles where the points are, that will show up when a user hovers near them
        const circles = points.map(point => this.createCircleForPoint(point));

        // Draw rectanges behind the points that act as user interactivity areas
        const hoverRects = points.map(point => this.createHoverAreaForPoint(point));

        // Draw a line from top to bottom through the point that will show upon
        // user hovering over the rectangle, and display the value and date
        const valueLines = points.map(point => this.createValueLineForPoint(point));

        // Draw lines in between each pair of points
        //const chunkedPoints = chunk(points, 2);
        const graphLines = [];
        let previousPoint = new GraphPoint(0, points[0].Y, points[0].Date);
        points.forEach((point, index) => {
            const newPoint = new GraphPoint(index, point.Y, point.Date);
            graphLines.push(this.createLineForTwoPoints(previousPoint, newPoint));
            previousPoint = newPoint;
        });

        // Attach all the SVG elements to the group, then the groups to the graph
        for(let i = 0; i < groups.length; i++) {
            groups[i].appendChild(hoverRects[i]);
            groups[i].appendChild(valueLines[i]);
            groups[i].appendChild(graphLines[i]);
            groups[i].appendChild(circles[i]);
            this.#svg.appendChild(groups[i]);
        }

        // Draw rectangles behind every "collection" of points (i.e.: for a day,
        // pre-market, the day itself, and after hours; for a week, each day)
        // which will turn the stroke-opacity all the way up for the line in that
        // collection when hovered on
    }
}