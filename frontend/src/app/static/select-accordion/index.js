import findParentBySelector from '../../helpers/nodes';

export default class SelectAccordion {
  constructor(accordion) {
    let activeToggler = {};
    const activeTabClass = 'isActive';
    const itemClass = 'js-accordion-group';
    const togglerClass = 'js-select-accordion-item';
    const togglers = Array.from(document.querySelectorAll(`.${togglerClass}`));
    const containers = Array.from(accordion.querySelectorAll(`.${itemClass}`));
    const inputs = Array.from(accordion.querySelectorAll('input'));


    togglers.forEach((toggler) => {
      toggler.addEventListener('click', (e) => {
        console.log('click');
        if (activeToggler !== e.target) {
          activeToggler = e.target;

          SelectAccordion.unstyleItems(containers, activeTabClass);

          SelectAccordion.disableCheckboxes(inputs, togglerClass);

          SelectAccordion.styleActiveItem(e.target, itemClass, activeTabClass);

        }
      });
    });
  }

  static unstyleItems(items, cls) {
    items.forEach((item) => {
      item.classList.remove(cls);
    });
  }

  static styleActiveItem(trigger, parentcls, addCls) {
    const item = findParentBySelector(trigger, `.${parentcls}`);
    item.classList.add(addCls);
  }

  static disableCheckboxes(inputs, exceptClass) {
    inputs.forEach((input) => {
      if (!input.classList.contains(exceptClass)) {
        input.checked = false;
      }
    });
  }
}
