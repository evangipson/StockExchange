import { coinFlip } from "../utils/numberUtils.js";
import { GraphConstants } from "./realTimeGraph.js";
import { chunk, average } from '../utils/jinq.js';

const getDayOfData = (initialPrice, dayDifference = 0) => {
    const now = new Date();
    let date = now;
    let price = initialPrice;
    return Array.from(Array(8).keys()).map(index => {
        price += Math.random() > 0.5 ? Math.random() : Math.random() * -1;
        price = Math.min(Math.max(0, price), 99999);
        date.setHours(now.getHours() - (1 + (8 * dayDifference)));
        date = new Date(date);
        return new CompanyPriceData(date, price);
    });
};

const getMockCompanyPriceData = () => {
    let price = (Math.random() * 5) + (Math.random() * 5);

    const yearsOfData = Math.floor(Math.random() * 3);
    if(yearsOfData) {
        return Array.from(Array(yearsOfData * 365).keys()).flatMap((data, index) => getDayOfData(price, index));
    }

    const monthsOfData = Math.floor(Math.random() * 12);
    if(monthsOfData) {
        return Array.from(Array(monthsOfData * 29).keys()).flatMap((data, index) => getDayOfData(price, index));
    }

    const daysOfData = Math.floor(Math.random() * 28 + 1);
    return Array.from(Array(daysOfData).keys()).flatMap((data, index) => getDayOfData(price, index));
};

class CompanyPriceData {
    #date;
    #amount;

    /**
     * 
     * @param {Date} date 
     * @param {Number} amount 
     */
    constructor(date, amount) {
        this.#date = date;
        this.#amount = amount;
    }

    get Price() {
        return this.#amount;
    }

    get Date() {
        return this.#date;
    }
}

export class Company {
    #name;
    #stockAfterHours;
    #historicalData;
    #dataByPrice;
    #dataByDate;

    /**
     * Creates a new company with or without data.
     * If no data is provided, mocks the data.
     * @param {string} name 
     * @param {CompanyPriceData[]|null} data 
     */
    constructor(name, data = null) {
        this.#name = name;
        this.#historicalData = data ?? getMockCompanyPriceData();
        this.#stockAfterHours = coinFlip() ? Math.random() * 5 : Math.random() * -5;
        this.#dataByPrice = this.#historicalData.toSorted((a, b) => a.Price - b.Price);
        this.#dataByDate = this.#historicalData.toSorted((a, b) => a.Date - b.Date);
    }

    get Name() {
        return this.#name;
    }

    get AllPriceData() {
        return this.#historicalData;
    }

    get StockValue() {
        return this.#dataByPrice[0].Price;
    }

    get StockChange() {
        return this.#dataByPrice[0].Price - this.#dataByPrice[this.#dataByPrice.length - 1].Price;
    }

    get DaysOfData() {
        const diffTime = Math.abs(this.#dataByDate[0].Date - this.#dataByDate[this.#dataByDate.length - 1].Date);
        const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24)); 
        return diffDays;
    }

    get DataByDate() {
        return this.#dataByDate;
    }

    get DataByPrice() {
        return this.#dataByPrice;
    }

    getAverageDataInChunks(daysToShow = 1) {
        const filteredDayData = this.getDataForDays(daysToShow);
        const chunksOfDays = chunk(filteredDayData, filteredDayData.length / GraphConstants.maxGraphNodes);
        const averagedChunks = chunksOfDays.map(chunk => {
            const averagePrice = average(chunk.map(item => item.Price));
            let dateTotal = 0;
            chunk.forEach(item => {
                dateTotal += item.Date.getTime();
            });
            dateTotal = dateTotal / chunk.length;
            const averageDate = new Date();
            averageDate.setTime(dateTotal);

            return new CompanyPriceData(new Date(averageDate), averagePrice);
        });
        return averagedChunks;
    }

    getDataForDays(daysAmount = 1) {
        const dateOffset = (24*60*60*1000) * daysAmount;
        const now = new Date();
        now.setTime(now.getTime() - dateOffset);
        return this.#dataByDate.filter(data => data.Date.getTime() >= now.getTime());
    }

    getStockChangeByDays(dayAmount = 1) {
        const todaysValues = this.getDataForDays(dayAmount);
        return todaysValues[todaysValues.length - 1].Price - todaysValues[0].Price;
    }

    getStockChangeByWeeks(weekAmount = 1) {
        const thisWeeksValues = this.getDataForDays(weekAmount * 7).sort(data => data.Price);
        return thisWeeksValues[0].Price - thisWeeksValues[thisWeeksValues.length - 1].Price;
    }

    getStockChangeByMonths(monthAmount = 1) {
        const thisMonthsValues = this.getDataForDays(monthAmount * 29).sort(data => data.Price);
        return thisMonthsValues[0].Price - thisMonthsValues[thisMonthsValues.length - 1].Price;
    }

    getStockChangeByYears(yearAmount = 1) {
        const thisYearsValues = this.getDataForDays(yearAmount * 365).sort(data => data.Price);
        return thisYearsValues[0].Price - thisYearsValues[thisYearsValues.length - 1].Price;
    }

    get StockAfterHours() {
        return this.#stockAfterHours;
    }
}