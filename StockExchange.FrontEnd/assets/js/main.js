import { showCompanyData, drawCompanyGraph } from './modules/graph.js';
import { replaceSelects } from './modules/select.js';

replaceSelects();
showCompanyData().then(company => {
    drawCompanyGraph(company);
});