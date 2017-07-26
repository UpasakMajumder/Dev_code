// @flow
import { getSearchObj, createNewUrl } from 'app.helpers/location';
import { consoleException } from 'app.helpers/io';

export default class Tabs {
  container: HTMLElement;
  activeClass: string;
  showClass: string;
  activeTab: ?HTMLElement;

  constructor(container: HTMLElement) {
    this.container = container;

    const { tab: tabQuery } = getSearchObj();

    const { tabActiveDefault, tab: tabSelector } = this.container.dataset;

    this.activeClass = 'active';
    this.showClass = 'show';

    const activeTab: ?HTMLElement = tabQuery
      ? this.container.querySelector(`[data-id="${tabQuery}"]`)
      : null;

    this.activeTab = activeTab || this.container.querySelector(`[data-tab-content="${tabActiveDefault}"]`);

    this.styleActiveTab();

    const tabs: HTMLElement[] = Array.from(this.container.querySelectorAll(tabSelector));

    tabs.forEach((tab: EventTarget) => {
      tab.addEventListener('click', (event: Event): void => {
        const target: EventTarget = event.target;

        if (!(target instanceof HTMLElement)) return;
        if (target === this.activeTab) return;

        this.unstyleActiveTab();
        this.activeTab = target;
        this.styleActiveTab();

        const { id } = target.dataset;
        const newUrl = createNewUrl({ search: {
          method: 'set',
          props: {
            tab: id
          }
        } });

        history.pushState({}, '', newUrl);
      });
    });
  }

  styleActiveTab(): void {
    if (!this.activeTab) {
      consoleException('Undefined tab');
      return;
    }

    this.activeTab.classList.add(this.activeClass);
    const content: ?HTMLElement = this.findContent();

    if (content) {
      setTimeout(() => {
        content.classList.add(this.activeClass);
      }, 301);

      setTimeout(() => {
        content.classList.add(this.showClass);
      }, 310);
    }
  }

  unstyleActiveTab(): void {
    if (!this.activeTab) {
      consoleException('Undefined tab');
      return;
    }

    this.activeTab.classList.remove(this.activeClass);
    const content = this.findContent();

    if (content) {
      content.classList.remove(this.showClass);

      setTimeout(() => {
        content.classList.remove(this.activeClass);
      }, 300);
    }
  }

  findContent(): ?HTMLElement {
    if (!this.activeTab) {
      consoleException('Undefined tab');
      return null;
    }

    const selector: ?string = this.activeTab.dataset.tabContent;

    if (!selector) {
      consoleException('Cannot find the content block, no data-tab-content');
      return null;
    }

    return this.container.querySelector(selector);
  }
}
