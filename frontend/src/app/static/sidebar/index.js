export default class Sidebar {
  constructor(container) {
    this.container = container;
    this.animatedClass = 'isFixed';
    this.containerOffsetTop = container.parentNode.offsetTop;
    this.containerHeight = container.offsetHeight;
    this.screenVisibleHeight = window.innerHeight - this.containerOffsetTop;

    window.addEventListener('scroll', () => {
      this.scroll();
    });
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
