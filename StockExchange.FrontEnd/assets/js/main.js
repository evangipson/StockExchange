import { showCompanyData, drawCompanyGraph } from "./modules/graph.js";

showCompanyData().then(company => {
    drawCompanyGraph(company);
});