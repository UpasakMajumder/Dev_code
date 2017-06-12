class Pagination {

  constructor(e, data) {
    this.code = '';
    this.Extend(data);
    this.Create(e);
    this.Start();
  }

  // --------------------
  // Utility
  // --------------------

  // converting initialize data
  Extend(data) {
    data = data || {};
    this.size = data.size || 300;
    this.page = data.page || 1;
    this.step = data.step || 3;
    this.callback = data.callback;
    this.wrapper = data.wrapper;
    this.toPage = data.toPage;
    this.fromPage = data.fromPage;
    this.rowsOnPage = data.rowsOnPage;
  }

  // add pages by number (from [s] to [f])
  Add(s, f) {
    for (let i = s; i < f; i += 1) {
      this.code += `<p data-page-number="${i}">${i}</p>`;
    }
  }

  // add last page with separator
  Last() {
    this.code += `<i>...</i><p> ${this.size}</p>`;
  }

  // add first page with separator
  First() {
    this.code += '<p>1</p><i>...</i>';
  }

  // --------------------
  // Handlers
  // --------------------

  // change page
  Click(event) {
    this.prevPage = this.page;
    this.page = +event.target.innerHTML;
    this.Start();
  }

  // previous page
  Prev() {
    this.prevPage = this.page;
    this.page -= 1;
    if (this.page < 1) {
      this.page = 1;
    }
    this.Start();
  }

  // next page
  Next() {
    this.prevPage = this.page;
    this.page += 1;
    if (this.page > this.size) {
      this.page = this.size;
    }
    this.Start();
  }

  // --------------------
  // Script
  // --------------------

  // binding pages
  Bind() {
    const a = this.e.getElementsByTagName('p');
    for (let i = 0; i < a.length; i += 1) {
      if (+a[i].innerHTML === this.page) a[i].className = 'generated-paginator__current';
      a[i].addEventListener('click', event => this.Click.call(this, event), false);
    }
    this.callback(this.prevPage, this.page, this.wrapper, this.toPage, this.fromPage, this.rowsOnPage);
  }

  // write pagination
  Finish() {
    this.e.innerHTML = this.code;
    this.code = '';
    this.Bind();
  }

  // find pagination type
  Start() {
    if (this.size < (this.step * 2) + 6) {
      this.Add(1, this.size + 1);
    } else if (this.page < (this.step * 2) + 1) {
      this.Add(1, (this.step * 2) + 4);
      this.Last();
    } else if (this.page > this.size - (this.step * 2)) {
      this.First();
      this.Add(this.size - (this.step * 2) - 2, this.size + 1);
    } else {
      this.First();
      this.Add(this.page - this.step, (this.page + this.step + 1));
      this.Last();
    }
    this.Finish();
  }

  // binding buttons
  Buttons(e) {
    const nav = e.getElementsByTagName('p');
    nav[0].addEventListener('click', this.Prev.bind(this), false);
    nav[1].addEventListener('click', this.Next.bind(this), false);
  }

  // create skeleton
  Create(e) {
    const html = [
      '<p class="generated-paginator__prev">&#60; Previous</p>', // previous button
      '<span></span>', // pagination container
      '<p class="generated-paginator__next">Next &#62;</p>' // next button
    ];

    e.innerHTML = html.join('');
    this.e = e.getElementsByTagName('span')[0];
    this.Buttons(e);
  }
}

export default Pagination;
