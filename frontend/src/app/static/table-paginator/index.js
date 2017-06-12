import Pagination from '../paginator';

class TablePaginator {
  constructor(tableWrapper) {
    this.tableWrapper = tableWrapper;
    this.startPageNumber = 1;
    const paginatorPageFromClass = 'js-table-paginator-from';
    const paginatorPageToClass = 'js-table-paginator-to';
    const paginatorWrapperClass = 'js-table-paginator-wrapper';
    const paginatorFromPage = this.tableWrapper.querySelector(`.${paginatorPageFromClass}`);
    const paginatorToPage = this.tableWrapper.querySelector(`.${paginatorPageToClass}`);
    const paginatorWrapper = this.tableWrapper.querySelector(`.${paginatorWrapperClass}`);
    const paginatorPagesNumber = +paginatorWrapper.dataset.pages;
    const paginatorRowsToShow = +paginatorWrapper.dataset.rowsOnPage;

    if (!paginatorWrapper) return;

    const init = () => {
      const paginator = new Pagination(paginatorWrapper, {
        size: paginatorPagesNumber, // pages size
        page: this.startPageNumber, // selected page
        step: 1, // pages before and after current
        callback: TablePaginator.callback,
        wrapper: this.tableWrapper,
        toPage: paginatorToPage,
        fromPage: paginatorFromPage,
        rowsOnPage: paginatorRowsToShow
      });
    };

    init();
  }

  static callback(prevPage, currPage, wrapper, to, from, rowsOnPage) {

    const rowActiveClass = 'active';
    const fromNumber = (((currPage - 1) * rowsOnPage) + 1);
    const toNumber = (currPage * rowsOnPage);

    const findRows = (num, wr) => {
      return Array.from(wr.querySelectorAll(`tr[data-page="${num}"`));
    };

    const unstyleActiveRows = (rows) => {
      rows.forEach((row) => {
        row.classList.remove(rowActiveClass);
      });
    };

    const styleActiveRows = (rows) => {
      rows.forEach((row) => {
        row.classList.add(rowActiveClass);
      });
    };

    const prevRows = findRows(prevPage, wrapper);
    const nextRows = findRows(currPage, wrapper);

    unstyleActiveRows(prevRows);
    styleActiveRows(nextRows);

    from.innerHTML = fromNumber;
    to.innerHTML = toNumber;

  }
}

export default TablePaginator;
