const formatAsCurrency = (amount, includePositiveIndicator = false) => {
    const usCurrencyFormat = ['en-US', { style: 'currency', currency: 'USD' }];
    const formattedAmount = new Intl.NumberFormat(...usCurrencyFormat).format(amount);
    return includePositiveIndicator ? `+${formattedAmount}` : formattedAmount;
};

export { formatAsCurrency }