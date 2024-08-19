const toggleLoader = (element) => {
    const elementIsLoading = Boolean(element.classList.contains('stock-exchange--loading'));
    element.classList.toggle('stock-exchange--loading', !elementIsLoading);
};

export { toggleLoader }