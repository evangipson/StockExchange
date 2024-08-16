import { RealTimeGraph } from "../classes/realTimeGraph.js";
import { fakeServerResponseTime } from "../utils/apiUtils.js";
import { formatAsCurrency } from "../utils/currencyUtils.js";
import { Company } from "../classes/company.js";

let companyGraph;

const fetchCompanyData = async () => {
    await fakeServerResponseTime(Math.random() * 2000);
    return new Company('Microsoft');
};

const createGraphPeriods = (company) => {
    const graphPeriods = [];
    const graphPeriodList = document.querySelector('.stock-exchange__graph-period');
    graphPeriodList.innerHTML = '';

    if(company.DaysOfData > 0) {
        const oneDay = document.createElement('LI');
        oneDay.setAttribute('data-days', 1);
        oneDay.setAttribute('data-stock-change', 'Today');
        oneDay.innerText = '1D';
        graphPeriods.push(oneDay);
    }

    if(company.DaysOfData > 6) {
        const oneWeek = document.createElement('LI');
        oneWeek.setAttribute('data-days', 7);
        oneWeek.setAttribute('data-stock-change', 'Past week');
        oneWeek.innerText = '1W';
        graphPeriods.push(oneWeek);
    }

    if(company.DaysOfData > 29) {
        const oneMonth = document.createElement('LI');
        oneMonth.setAttribute('data-days', 29);
        oneMonth.setAttribute('data-stock-change', 'Past month');
        oneMonth.innerText = '1M';
        graphPeriods.push(oneMonth);
    }

    if(company.DaysOfData > 29 * 3) {
        const threeMonths = document.createElement('LI');
        threeMonths.setAttribute('data-days', 29 * 3);
        threeMonths.setAttribute('data-stock-change', 'Past 3 months');
        threeMonths.innerText = '3M';
        graphPeriods.push(threeMonths);
    }

    const yearToDate = document.createElement('LI');
    let firstOfTheYear = new Date();
    firstOfTheYear = new Date(firstOfTheYear.setMonth(0));
    firstOfTheYear = new Date(firstOfTheYear.setDate(1));
    const differenceInDays = Math.round((new Date().getTime() - firstOfTheYear.getTime()) / (1000 * 3600 * 24));
    yearToDate.setAttribute('data-days', differenceInDays);
    yearToDate.setAttribute('data-stock-change', `Past ${differenceInDays} days`);
    yearToDate.innerText = 'YTD';
    graphPeriods.push(yearToDate);

    if(company.DaysOfData > 364) {
        const oneYear = document.createElement('LI');
        oneYear.setAttribute('data-days', 365);
        oneYear.setAttribute('data-stock-change', 'Past year');
        oneYear.innerText = '1Y';
        graphPeriods.push(oneYear);
    }

    if(company.DaysOfData > 364 * 5) {
        const fiveYears = document.createElement('LI');
        fiveYears.setAttribute('data-days', 365 * 5);
        fiveYears.setAttribute('data-stock-change', 'Past 5 years');
        fiveYears.innerText = '5Y';
        graphPeriods.push(fiveYears);
    }

    graphPeriods.forEach((period, index) => {
        period.addEventListener('click', () => {
            graphPeriods.forEach(graphPeriod => graphPeriod.classList.remove('stock-exchange__graph-period-item--active'));
            period.classList.add('stock-exchange__graph-period-item--active');
            drawCompanyGraph(company, period.getAttribute('data-days'));
            updateCompanyData(company, period.getAttribute('data-days'));
        });
        graphPeriodList.appendChild(period);
        if(index == 0) {
            period.click();
        }
    });
};

const showCompanyData = async () => {
    const company = await fetchCompanyData();
    createGraphPeriods(company);
    return company;
};

/**
 * 
 * @param {Company} company 
 * @param {Number} daysToShow 
 */
const updateCompanyData = (company, daysToShow = 1) => {
    const stockChange = company.getStockChangeByDays(daysToShow);
    const valueIncreased = stockChange > 0.0;
    const companyValue = document.querySelector('.stock-exchange__company-stock-value');
    const companyChange = document.querySelector('.stock-exchange__company-stock-change');
    const companyAfterHours = document.querySelector('.stock-exchange__company-stock-after-hours');
    document.querySelector('.stock-exchange__company-name').innerText = company.Name;
    companyValue.innerText = formatAsCurrency(company.StockValue);
    companyChange.innerText = formatAsCurrency(stockChange, valueIncreased);
    companyAfterHours.innerText = formatAsCurrency(company.StockAfterHours, company.StockAfterHours > 0.0);

    companyChange.classList.toggle('stock-exchange--positive', valueIncreased);
    companyChange.classList.toggle('stock-exchange--negative', !valueIncreased);
    companyAfterHours.classList.toggle('stock-exchange--positive', company.StockAfterHours > 0.0);
    companyAfterHours.classList.toggle('stock-exchange--negative', company.StockAfterHours < 0.0);

    // add data-text or w/e from the list items as ::after? or maybe just supplement innerText...
};

const drawCompanyGraph = (company, daysToShow = 1) => {
    const graphCanvas = document.querySelector('.stock-exchange__graph-canvas');
    const newCompanyGraph = companyGraph || new RealTimeGraph(graphCanvas);
    newCompanyGraph.updateGraph(company, daysToShow);
};

export { showCompanyData, drawCompanyGraph }