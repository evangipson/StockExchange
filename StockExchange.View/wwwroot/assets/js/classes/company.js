import { coinFlip } from "../utils/numberUtils.js";
import { GraphConstants } from "./graph.js";
import { chunk } from '../utils/collectionUtils.js';

const getDayOfData = (initialPrice, dayDifference = 0) => {
    const now = new Date();
    now.setHours(8);
    let date = now;
    let price = initialPrice;

    // 36 15 minute intervals in 9 hours (8am-5pm)
    return Array.from(Array(36).keys()).map(index => {
        price += Math.random() > 0.5 ? Math.random() : Math.random() * -1;
        price = Math.min(Math.max(0, price), 99999);
        date.setMinutes(now.getMinutes() - (15 + (9 * dayDifference)));
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

export class CompanyPriceData {
    #date;
    #amount;

    /**
     * 
     * @param {Number} amount 
     * @param {Date} date 
     */
    constructor(amount, date) {
        this.#amount = amount;
        this.#date = new Date(date);
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
    #ticker;
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
    constructor(name, ticker, data = null) {
        this.#name = name;
        this.#ticker = ticker;
        this.#historicalData = data ?? getMockCompanyPriceData();
        this.#stockAfterHours = coinFlip() ? Math.random() * 3 : Math.random() * -3;
        this.#dataByPrice = this.#historicalData.toSorted((a, b) => b.Price - a.Price);
        this.#dataByDate = this.#historicalData.toSorted((a, b) => b.Date - a.Date);
    }

    get Name() {
        return this.#name;
    }

    get Ticker() {
        return this.#ticker;
    }

    get AllPriceData() {
        return this.#historicalData;
    }

    get StockValue() {
        return this.#dataByDate[0].Price;
    }

    get StockChange() {
        return this.#dataByPrice[0].Price - this.#dataByPrice[this.#dataByPrice.length - 1].Price;
    }

    get DaysOfData() {
        const diffTime = Math.abs(this.#dataByDate[0].Date - this.#dataByDate[this.#dataByDate.length - 1].Date);
        const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));
        return diffDays ? diffDays : 1;
    }

    get DataByDate() {
        return this.#dataByDate;
    }

    get DataByPrice() {
        return this.#dataByPrice;
    }

    get StockAfterHours() {
        return this.#stockAfterHours;
    }

    getAverageDataInChunks(daysToShow = 1) {
        const filteredDayData = this.getDataForDays(daysToShow);
        const chunksOfDays = chunk(filteredDayData, filteredDayData.length / GraphConstants.maxGraphNodes);
        const averagedChunks = chunksOfDays.map((chunk, index) => {
            const chunkToTake = index === 0 ? 0 : chunk.length - 1;
            return new CompanyPriceData(chunk[chunkToTake].Date, chunk[chunkToTake].Price);
        });
        return averagedChunks;
    }

    getDataForDays(daysAmount = 1) {
        const dateOffset = (24*60*60*1000) * daysAmount;
        const now = new Date(new Date().getTime() - dateOffset);
        return this.#dataByDate.filter(data => data.Date.getTime() > now.getTime()).reverse();
    }

    getStockChangeByDays(dayAmount = 1) {
        const todaysValues = this.getDataForDays(dayAmount);
        return todaysValues[todaysValues.length - 1].Price - todaysValues[0].Price;
    }

    getStockChangeByWeeks(weekAmount = 1) {
        const thisWeeksValues = this.getDataForDays(weekAmount * 7).sort(data => data.Price);
        return thisWeeksValues[thisWeeksValues.length - 1].Price - thisWeeksValues[0].Price;
    }

    getStockChangeByMonths(monthAmount = 1) {
        const thisMonthsValues = this.getDataForDays(monthAmount * 29).sort(data => data.Price);
        return thisMonthsValues[thisMonthsValues.length - 1].Price - thisMonthsValues[0].Price;
    }

    getStockChangeByYears(yearAmount = 1) {
        const thisYearsValues = this.getDataForDays(yearAmount * 365).sort(data => data.Price);
        return thisYearsValues[thisYearsValues.length - 1].Price - thisYearsValues[0].Price;
    }
}