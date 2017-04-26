export default class Sidebar {
  constructor(container) {
    this.container = container;
    this.animatedClass = 'isFixed';
    this.containerOffsetTop = container.getBoundingClientRect().top;
    this.containerHeight = container.offsetHeight;
    this.screenVisibleHeight = window.innerHeight - this.containerOffsetTop;
    this.initScroll();
    this.resize();
  }

  initScroll() {
    window.addEventListener('scroll', () => {
      if (window.scrollY < this.containerOffsetTop || this.containerHeight > this.screenVisibleHeight) {
        this.container.classList.remove(this.animatedClass);
      } else {
        this.container.classList.add(this.animatedClass);
      }
    });
  }

  resize() {
    window.addEventListener('resize', () => {
      this.screenVisibleHeight = window.innerHeight - this.containerOffsetTop;
    });
  }

}
