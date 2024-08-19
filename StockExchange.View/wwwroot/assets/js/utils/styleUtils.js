const getProperty = (propertyName) => {
    const computedStyle = window.getComputedStyle(document.body);
    return computedStyle.getPropertyValue(`--${propertyName}`);
};

export { getProperty }