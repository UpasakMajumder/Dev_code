export default class Tabs {
  constructor(container) {
    this.container = container;
    const { hash } = location;
    const { tabActiveDefault, tab: tabSelector } = this.container.dataset;

    this.activeClass = 'active';
    this.showClass = 'show';

    const activeTab = this.container.querySelector(`[data-id="${hash}"]`);

    this.activeTab = activeTab || this.container.querySelector(`[data-tab-content="${tabActiveDefault}"]`);

    this.styleActiveTab();

    const tabs = Array.from(this.container.querySelectorAll(tabSelector));

    tabs.forEach((tab) => {
      tab.addEventListener('click', (event) => {
        const target = event.target;

        if (target === this.activeTab) return;

        this.unstyleActiveTab();
        this.activeTab = target;
        this.styleActiveTab();

        const { id } = target.dataset;
        location.hash = id;
      });
    });
  }

  styleActiveTab() {
    this.activeTab.classList.add(this.activeClass);
    const content = this.findContent(this.activeTab);

    if (content) {
      setTimeout(() => {
        content.classList.add(this.activeClass);
      }, 301);

      setTimeout(() => {
        content.classList.add(this.showClass);
      }, 310);
    }
  }

  unstyleActiveTab() {
    this.activeTab.classList.remove(this.activeClass);
    const content = this.findContent(this.activeTab);

    if (content) {
      content.classList.remove(this.showClass);

      setTimeout(() => {
        content.classList.remove(this.activeClass);
      }, 300);
    }
  }

  findContent(tab) {
    const selector = tab.dataset.tabContent;
    return this.container.querySelector(selector);
  }
}
