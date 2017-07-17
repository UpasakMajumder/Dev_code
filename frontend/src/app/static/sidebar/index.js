// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

export default class Sidebar {
  container: HTMLElement;
  animatedClass: string;
  containerOffsetTop: number;
  containerHeight: number;
  screenVisibleHeight: number;

  constructor(container: HTMLElement) {
    if (!container.parentNode || !(container.parentNode instanceof HTMLElement)) {
      consoleException('Container has no parent node');
      return;
    }

    this.container = container;
    this.animatedClass = 'isFixed';
    this.containerOffsetTop = container.parentNode.offsetTop;
    this.containerHeight = container.offsetHeight;
    this.screenVisibleHeight = window.innerHeight - this.containerOffsetTop;

    window.addEventListener('scroll', this.scroll.bind(this));
    this.scroll();
    this.resize();
  }

  scroll() {
    if (window.pageYOffset < this.containerOffsetTop || this.containerHeight > this.screenVisibleHeight) {
      this.container.classList.remove(this.animatedClass);
    } else {
      this.container.classList.add(this.animatedClass);
    }
  }

  resize() {
    window.addEventListener('resize', () => {
      this.screenVisibleHeight = window.innerHeight - this.containerOffsetTop;
    });
  }
}
