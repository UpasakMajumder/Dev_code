import separate from '../../helpers/numbers';

export default class MoneyFormat {
  constructor(container) {
    const str = container.innerHTML;
    const symbolCents = '.';
    const symbolDigits = ',';

    const separatedBy = str.split(symbolCents);
    const strBeforeDot = separatedBy[0];
    const strAfterDot = separatedBy[1];

    let value = separate(strBeforeDot, symbolDigits);

    if (strAfterDot) value += `.${strAfterDot}`;
    container.innerHTML = value;
  }
}
