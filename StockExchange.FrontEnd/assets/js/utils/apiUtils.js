/**
 * Will fake a server response time. Used in the case of
 * mocked responses.
 * @param {Number} timeToWait - the amount of time to wait,
 * in milliseconds, defaults to 1000 milliseconds.
 */
const fakeServerResponseTime = async (timeToWait = 2500) => {
    await new Promise(fakeServerResponse => setTimeout(fakeServerResponse, timeToWait));
};

export { fakeServerResponseTime }