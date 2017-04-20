export default class Sidebar {
  constructor(container) {
    this.container = container;
    this.startSidebarPos = -1;
    this.animatedClass = 'isFixed';
    this.initScroll();
  }

  initScroll() {
    window.addEventListener('scroll', () => {
      const bar = this.container;
      if (this.startSidebarPos < 0) {
        this.startSidebarPos = Sidebar.findPosY(bar);
      }

      if (window.pageYOffset > this.startSidebarPos) {
        bar.classList.add(this.animatedClass);
      } else {
        bar.classList.remove(this.animatedClass);
      }

    });
  }

  static findPosY(obj) {
    let curtop = 0;
    if (typeof obj.offsetParent !== 'undefined' && obj.offsetParent) {
      while (obj.offsetParent) {
        curtop += obj.offsetTop;
        obj = obj.offsetParent;
      }
      curtop += obj.offsetTop;
    } else if (obj.y) {
      curtop += obj.y;
    }
    return curtop;
  }

}
