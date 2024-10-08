import { formatAsCurrency } from "../utils/currencyUtils.js";
import { getDaysFromYearStart } from "../utils/dateUtils.js";
import { toggleLoader } from "../utils/loadingUtils.js";
import { fetchCompanyData } from "../api/companyApi.js";
import { Company, CompanyPriceData } from "../classes/company.js";
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

/**
 * 
 * @param {Company} company 
 */
const createGraphPeriods = (company) => {
    const graphPeriodList = document.querySelector('.stock-exchange__graph-period');
    const periods = [
        { days: 1, message: 'Today', listText: '1 D' },
        { days: 6, message: 'Past week', listText: '1 W' },
        { days: 29, message: 'Past month', listText: '1 M' },
        { days: (29 * 3), message: 'Past 3 months', listText: '3 M' },
        { days: getDaysFromYearStart(), message: `Past ${getDaysFromYearStart()} days`, listText: 'YTD'},
        { days: 364, message: 'Past year', listText: '1 Y' },
        { days: (364 * 3), message: 'Past 3 years', listText: '3 Y' },
        { days: (364 * 5), message: 'Past 5 years', listText: '5 Y' },
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
    const companyTicker = document.querySelector('.stock-exchange__transaction-header');
    const stockChange = company.getStockChangeByDays(daysToShow);
    const stockChangeIncreased = stockChange > 0.0;
    const showingOneDayOfData = daysToShow == 1;
    
    companyName.innerText = company.Name;
    companyTicker.innerText = `Buy ${company.Ticker}`;
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
        companyAfterHoursTime.innerText = 'After-hours';
        companyAfterHoursValue.classList.toggle('stock-exchange--positive', afterHoursIncreased);
        companyAfterHoursValue.classList.toggle('stock-exchange--negative', !afterHoursIncreased);
    }
};

const drawCompanyGraph = (company, daysToShow = 1) => {
    const graphCanvas = document.querySelector('.stock-exchange__graph-canvas');
    const companyValue = document.querySelector('.stock-exchange__company-stock-value');
    const newCompanyGraph = companyGraph || new Graph(graphCanvas);
    if(!companyGraph) {
        graphCanvas.addEventListener('pointerout', () => {
            const initialValue = companyValue.getAttribute('data-stock-value');
            companyValue.innerText = formatAsCurrency(initialValue);
        });
    }
    newCompanyGraph.updateGraph(company, daysToShow);
};

const showCompanyData = async () => {
    const graphData = document.querySelector('.stock-exchange__graph-data');
    const companyNameElement = document.querySelector('.stock-exchange__company-name'); 
    const companyStockPricesJSON = graphData?.getAttribute('data-graph-data');
    const companyName = companyNameElement?.getAttribute('data-company-name');
    const companyTicker = companyNameElement?.getAttribute('data-company-ticker');
    let company;
    let companyStockPrices;
    let stockPrices;

    toggleCompanyPageLoaders();

    if(!companyStockPricesJSON) {
        company = await fetchCompanyData();
    } else {
        companyStockPrices = JSON.parse(companyStockPricesJSON);
        stockPrices = companyStockPrices?.map(stockPrices => new CompanyPriceData(stockPrices.Amount, stockPrices.Date));
        company = new Company(companyName, companyTicker, stockPrices);
    }

    createGraphPeriods(company);

    toggleCompanyPageLoaders();

    const companyValue = document.querySelector('.stock-exchange__company-stock-value');
    companyValue.addEventListener('UpdateCompanyValue', (event) => {
        companyValue.innerText = formatAsCurrency(event.detail.newValue);
    });

    return company;
};

export { showCompanyData, drawCompanyGraph }