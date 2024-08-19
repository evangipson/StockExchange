import { showCompanyData, drawCompanyGraph } from './modules/companyPage.js';
import { replaceSelects } from './modules/select.js';

replaceSelects();

showCompanyData().then(company => {
    drawCompanyGraph(company);
});