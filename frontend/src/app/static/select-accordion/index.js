import findParentBySelector from '../../helpers/nodes';

export default class SelectAccordion {
  constructor(accordion) {
    const activeTabClass = 'isActive';
    const itemClass = 'js-accordion-group';
    const togglerClass = 'js-select-accordion-item';
    const togglers = Array.from(accordion.querySelectorAll(`.${togglerClass}`));
    const containers = Array.from(accordion.querySelectorAll(`.${itemClass}`));
    const inputs = Array.from(accordion.querySelectorAll('input'));
    let activeToggler = togglers[0];


    togglers.forEach((toggler) => {
      toggler.addEventListener('click', (e) => {
        if (activeToggler === e.target) return;

        SelectAccordion.unstyleItem(activeToggler, itemClass, activeTabClass);

        SelectAccordion.disableCheckboxes(inputs, togglerClass);

        activeToggler = e.target;

        SelectAccordion.styleActiveItem(e.target, itemClass, activeTabClass);

      });
    });
  }

  static unstyleItem(toggler, itemCls, cls) {
    const parent = findParentBySelector(toggler, `.${itemCls}`);
    parent.classList.remove(cls);
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
