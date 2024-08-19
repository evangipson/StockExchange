/**
 * Chunks an array into multiple, if provided a valid `chunkSize`.
 * @param {Number} chunkSize 
 * @returns An array, chunked into multiple arrays of equal size,
 * with the remainder as the last array, if any.
 */
const chunk = (array, chunkSize) => {
    // chunk size will default to 1 if provided a bogus value
    if (!chunkSize || typeof chunkSize != "number" || chunkSize <= 0) {
        chunkSize = 1;
    }

    return array.reduce((a, b, index) => {
        const chunk = Math.floor(index / chunkSize);
        a[chunk] = [].concat(a[chunk] || [], b);
        return a;
    }, []);
};

/**
 * Gets the average of all numbers of the array.
 * @returns - an average if called off of an array of numbers.
 */
const average = (array) => {
    const numberValues = array.filter((x) => x && typeof x == "number");

    if(numberValues.length) {
        return numberValues.reduce((a, b) => a + b);
    }
    return NaN;
};

export { chunk, average }