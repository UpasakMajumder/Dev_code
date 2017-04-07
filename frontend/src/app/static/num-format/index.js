import separate from '../../helpers/numbers';

export default class NumFormat {
  constructor(container) {
    const str = container.innerHTML;
    container.innerHTML = separate(str);
  }
}
