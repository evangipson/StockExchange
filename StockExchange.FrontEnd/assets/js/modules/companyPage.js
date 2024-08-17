import { formatAsCurrency } from "../utils/currencyUtils.js";
import { getDaysFromYearStart } from "../utils/dateUtils.js";
import { toggleLoader } from "../utils/loadingUtils.js";
import { fetchCompanyData } from "../api/companyApi.js";
import { Company } from "../classes/company.js";
import { Graph } from "../classes/graph.js";

let companyGraph;

const updateGraphPeriod = (company, graphPeriods, clickedPeriod) => {
    const companyChangeTime = document.querySelector('.stock-exchange__company-stock-change-time');
    const daysOfData = clickedPeriod.getAttribute('data-days') ?? 1;
    const stockChangeMessage = clickedPeriod.getAttribute('data-stock-change');

    graphPeriods.forEach(clickedPeriod => clickedPeriod.classList.remove('stock-exchange__graph-period-item--active'));
    clickedPeriod.classList.add('stock-exchange__graph-period-item--active');
    drawCompanyGraph(company, daysOfData);
    updateCompanyData(company, daysOfData);
    companyChangeTime.innerText = stockChangeMessage;
};

const createGraphPeriods = (company) => {
    const graphPeriodList = document.querySelector('.stock-exchange__graph-period');
    const periods = [
        { days: 1, message: 'Today', listText: '1D' },
        { days: 6, message: 'Past week', listText: '1W' },
        { days: 29, message: 'Past month', listText: '1M' },
        { days: (29 * 3), message: 'Past 3 months', listText: '3M' },
        { days: getDaysFromYearStart(), message: `Past ${getDaysFromYearStart()} days`, listText: 'YTD'},
        { days: 364, message: 'Past year', listText: '1Y' },
        { days: (364 * 3), message: 'Past 3 years', listText: '3Y' },
        { days: (364 * 5), message: 'Past 5 years', listText: '5Y' },
    ];
    const graphPeriods = [];

    graphPeriodList.innerHTML = '';
    for(const period in periods) {
        const currentPeriod = periods[period];

        if(company.DaysOfData < currentPeriod.days) {
            break;
        }
        
        const graphPeriodElement = document.createElement('LI');
        graphPeriodElement.setAttribute('data-days', currentPeriod.days);
        graphPeriodElement.setAttribute('data-stock-change', currentPeriod.message);
        graphPeriodElement.innerText = currentPeriod.listText;
        graphPeriods.push(graphPeriodElement);
    }

    graphPeriods.forEach((period) => {
        period.addEventListener('click', () => updateGraphPeriod(company, graphPeriods, period));
        graphPeriodList.appendChild(period);
    });

    graphPeriods[0].click();
};

const toggleCompanyPageLoaders = () => {
    const companyPageAsyncElements = document.querySelectorAll('.stock-exchange__graph-data, .stock-exchange__transaction-window');
    companyPageAsyncElements.forEach(asyncElement => toggleLoader(asyncElement));
};

/**
 * 
 * @param {Company} company 
 * @param {Number} daysToShow 
 */
const updateCompanyData = (company, daysToShow = 1) => {
    const companyName = document.querySelector('.stock-exchange__company-name');
    const companyValue = document.querySelector('.stock-exchange__company-stock-value');
    const transactionWindowValue = document.querySelector('.stock-exchange__transaction-company-price');
    const companyChange = document.querySelector('.stock-exchange__company-stock-change-value');
    const companyAfterHours = document.querySelector('.stock-exchange__company-after-hours');
    const stockChange = company.getStockChangeByDays(daysToShow);
    const stockChangeIncreased = stockChange > 0.0;
    const showingOneDayOfData = daysToShow == 1;
    
    companyName.innerText = company.Name;
    companyValue.innerText = formatAsCurrency(company.StockValue);
    transactionWindowValue.innerText = formatAsCurrency(company.StockValue);
    companyChange.innerText = formatAsCurrency(stockChange, stockChangeIncreased);
    companyChange.classList.toggle('stock-exchange--positive', stockChangeIncreased);
    companyChange.classList.toggle('stock-exchange--negative', !stockChangeIncreased);
    companyAfterHours.classList.toggle('stock-exchange__company-after-hours--active', showingOneDayOfData);

    if(showingOneDayOfData) {
        const companyAfterHoursValue = companyAfterHours.querySelector('.stock-exchange__company-after-hours-value');
        const companyAfterHoursTime = companyAfterHours.querySelector('.stock-exchange__company-after-hours-time');
        const afterHoursIncreased = company.StockAfterHours > 0.0;

        companyAfterHoursValue.innerText = formatAsCurrency(company.StockAfterHours, afterHoursIncreased);
        companyAfterHoursTime.innerText = 'after hours';
        companyAfterHoursValue.classList.toggle('stock-exchange--positive', afterHoursIncreased);
        companyAfterHoursValue.classList.toggle('stock-exchange--negative', !afterHoursIncreased);
    }
};

const drawCompanyGraph = (company, daysToShow = 1) => {
    const graphCanvas = document.querySelector('.stock-exchange__graph-canvas');
    const newCompanyGraph = companyGraph || new Graph(graphCanvas);
    newCompanyGraph.updateGraph(company, daysToShow);
};

const showCompanyData = async () => {
    let company;

    toggleCompanyPageLoaders();
    company = await fetchCompanyData();
    createGraphPeriods(company);

    toggleCompanyPageLoaders();
    return company;
};

export { showCompanyData, drawCompanyGraph }