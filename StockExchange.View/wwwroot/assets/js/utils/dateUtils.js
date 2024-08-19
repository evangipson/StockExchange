const getDaysFromYearStart = () => {
    let firstOfTheYear = new Date();
    firstOfTheYear = new Date(firstOfTheYear.setMonth(0));
    firstOfTheYear = new Date(firstOfTheYear.setDate(1));
    return Math.round((new Date().getTime() - firstOfTheYear.getTime()) / (1000 * 3600 * 24));
};

export { getDaysFromYearStart }