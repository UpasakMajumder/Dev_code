export default class Tabs {
  constructor(container) {
    this.container = container;
    const { tabActiveDefault, tab } = this.container.dataset;

    this.activeClass = 'active';
    this.showClass = 'show';

    this.activeTab = this.container.querySelector(`[data-tab-content="${tabActiveDefault}"]`);

    this.styleActiveTab();

    const tabs = Array.from(this.container.querySelectorAll(tab));

    tabs.forEach((tab) => {
      tab.addEventListener('click', (event) => {
        const target = event.target;

        if (target === this.activeTab) return;

        this.unstyleActiveTab();
        this.activeTab = target;
        this.styleActiveTab();
      });
    });
  }

  styleActiveTab() {
    this.activeTab.classList.add(this.activeClass);
    const content = this.findContent(this.activeTab);

    setTimeout(() => {
      content.classList.add(this.activeClass);
    }, 301);

    setTimeout(() => {
      content.classList.add(this.showClass);
    }, 310);
  }

  unstyleActiveTab() {
    this.activeTab.classList.remove(this.activeClass);
    const content = this.findContent(this.activeTab);
    content.classList.remove(this.showClass);

    setTimeout(() => {
      content.classList.remove(this.activeClass);
    }, 300);
  }

  findContent(tab) {
    const selector = tab.dataset.tabContent;
    return this.container.querySelector(selector);
  }
}
