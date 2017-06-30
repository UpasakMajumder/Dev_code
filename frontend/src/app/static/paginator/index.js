class Pagination {

  constructor(wrapper, data) {
    this.code = '';
    this.extend(data);
    this.create(wrapper);
    this.start();
  }

  // --------------------
  // Utility
  // --------------------

  // converting initialize data
  extend(data) {
    data = data || {};
    this.size = data.size || 300;
    this.page = data.page || 1;
    this.step = data.step || 3;
    this.callback = data.callback;
    this.container = data.container;
    this.toPage = data.toPage;
    this.fromPage = data.fromPage;
    this.rowsOnPage = data.rowsOnPage;
  }

  // add pages by number (from [s] to [f])
  add(s, f) {
    for (let i = s; i < f; i += 1) {
      this.code += `<p data-page-number="${i}">${i}</p>`;
    }
  }

  // add last page with separator
  last() {
    this.code += `<i>...</i><p> ${this.size}</p>`;
  }

  // add first page with separator
  first() {
    this.code += '<p>1</p><i>...</i>';
  }

  // --------------------
  // Handlers
  // --------------------

  // change page
  click(event) {
    this.prevPage = this.page;
    this.page = +event.target.innerHTML;
    this.start();
  }

  // previous page
  prev() {
    this.prevPage = this.page;
    this.page = (this.page <= 1) ? 1 : this.page - 1;
    this.start();
  }

  // next page
  next() {
    this.prevPage = this.page;
    this.page = (this.page >= this.size) ? this.size : this.page + 1;
    this.start();
  }

  // --------------------
  // Script
  // --------------------

  // binding pages
  bind() {
    const a = this.wrapper.getElementsByTagName('p');
    for (let i = 0; i < a.length; i += 1) {
      if (+a[i].innerHTML === this.page) a[i].className = 'generated-paginator__current';
      a[i].addEventListener('click', event => this.click.call(this, event), false);
    }
    this.callback(this.prevPage, this.page, this.container, this.toPage, this.fromPage, this.rowsOnPage);
  }

  // write pagination
  finish() {
    this.wrapper.innerHTML = this.code;
    this.code = '';
    this.bind();
  }

  // find pagination type
  start() {
    if (this.size < (this.step * 2) + 6) {
      this.add(1, this.size + 1);
    } else if (this.page < (this.step * 2) + 1) {
      this.add(1, (this.step * 2) + 4);
      this.last();
    } else if (this.page > this.size - (this.step * 2)) {
      this.first();
      this.add(this.size - (this.step * 2) - 2, this.size + 1);
    } else {
      this.first();
      this.add(this.page - this.step, (this.page + this.step + 1));
      this.last();
    }
    this.finish();
  }

  // binding buttons

  buttons(wrapper) {
    const nav = wrapper.getElementsByTagName('p');
    nav[0].addEventListener('click', this.prev.bind(this), false);
    nav[1].addEventListener('click', this.next.bind(this), false);
  }

  // create skeleton
  create(wrapper) {
    const html = [
      '<p class="generated-paginator__prev">Previous</p>', // previous button
      '<span></span>', // pagination container
      '<p class="generated-paginator__next">Next</p>' // next button
    ];

    wrapper.innerHTML = html.join('');
    this.wrapper = wrapper.getElementsByTagName('span')[0];
    this.buttons(wrapper);
  }
}

export default Pagination;
