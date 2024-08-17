import { fakeServerResponseTime } from "../utils/apiUtils.js";
import { Company } from "../classes/company.js";

const fetchCompanyData = async () => {
    await fakeServerResponseTime(Math.random() * 4000);
    return new Company('Microsoft');
};

export { fetchCompanyData }