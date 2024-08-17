const updateSelect = (select, options, activeOption) => {
    console.info('updating select with an option');
    const selectedOption = select.querySelector('.stock-exchange__input--selected-option');

    options.forEach(option => option.classList.remove('stock-exchange__input--active-option'));
    selectedOption.value = activeOption.innerText;
    activeOption.classList.add('stock-exchange__input--active-option');
    select.classList.remove('stock-exchange__input--select-active');
};

const replaceSelects = () => {
    const allSelects = document.querySelectorAll('.stock-exchange__input select');
    allSelects.forEach(select => {
        const options = select.querySelectorAll('option');
        const newSelect = document.createElement('div');
        const newSelectedOption = document.createElement('input');
        const newOptionWrapper = document.createElement('div');
        let newOptions = [];

        newSelect.setAttribute('tabindex', -1);
        newSelectedOption.type = 'text';
        newSelectedOption.readOnly = true;
        newSelectedOption.classList.add('stock-exchange__input--selected-option');
        newSelect.classList.add('stock-exchange__input--select');
        newOptionWrapper.classList.add('stock-exchange__input--option-list');

        options.forEach(option => {
            const newOption = document.createElement('div');
            newOption.classList.add('stock-exchange__input--option');
            newOption.innerText = option.innerText;
            if(option.selected) {
                newOption.classList.add('stock-exchange__input--active-option');
                newOption.setAttribute('data-selected', true);
                newSelectedOption.value = newOption.innerText;
                newSelectedOption.setAttribute('value', newOption.innerText);
            }
            newOptions.push(newOption);
        });

        newOptions.forEach(newOption => {
            newOption.addEventListener('click', () => {
                updateSelect(newSelect, newOptions, newOption);
            });
            newOptionWrapper.appendChild(newOption);
        });

        newSelectedOption.addEventListener('focus', () => newSelect.classList.add('stock-exchange__input--select-active'));
        newSelectedOption.addEventListener('blur', (blurEvent) => {
            if(!blurEvent.currentTarget.parentElement.contains(blurEvent.relatedTarget)) {
                console.info('blurring the select');
                newSelect.classList.remove('stock-exchange__input--select-active');
            }
        });

        newSelect.appendChild(newSelectedOption);
        newSelect.appendChild(newOptionWrapper);
        select.replaceWith(newSelect);
    });
};

export { replaceSelects }