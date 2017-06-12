import Pagination from '../paginator';

class TablePaginator {
  constructor(tableWrapper) {
    const startPageNumber = 1;
    const step = 1;
    const paginatorPageFromClass = 'js-table-paginator-from';
    const paginatorPageToClass = 'js-table-paginator-to';
    const paginatorWrapperClass = 'js-table-paginator-wrapper';
    const paginatorFromPage = tableWrapper.querySelector(`.${paginatorPageFromClass}`);
    const paginatorToPage = tableWrapper.querySelector(`.${paginatorPageToClass}`);
    const paginatorWrapper = tableWrapper.querySelector(`.${paginatorWrapperClass}`);

    const { pages: paginatorPagesNumber, rowsOnPage: paginatorRowsToShow } = paginatorWrapper.dataset;

    if (!paginatorWrapper) return;

    const init = () => {
      const paginator = new Pagination(paginatorWrapper, {
        size: +paginatorPagesNumber, // pages size
        page: startPageNumber, // selected page
        step, // pages before and after current
        callback: TablePaginator.callback,
        container: tableWrapper,
        toPage: paginatorToPage,
        fromPage: paginatorFromPage,
        rowsOnPage: +paginatorRowsToShow
      });
    };

    init();
  }

  static callback(prevPage, currPage, wrapper, to, from, rowsOnPage) {
    const rowActiveClass = 'active';
    const fromNumber = (((currPage - 1) * rowsOnPage) + 1);
    const toNumber = currPage * rowsOnPage;

    const findRows = (num) => {
      const rows = wrapper.querySelectorAll(`tr[data-page="${num}"`);
      return Array.from(rows);
    };

    const unstyleActiveRows = (rows) => {
      rows.forEach(row => row.classList.remove(rowActiveClass));
    };

    const styleActiveRows = (rows) => {
      rows.forEach(row => row.classList.add(rowActiveClass));
    };

    const prevRows = findRows(prevPage);
    const nextRows = findRows(currPage);

    unstyleActiveRows(prevRows);
    styleActiveRows(nextRows);

    from.innerHTML = fromNumber;
    to.innerHTML = toNumber;
  }
}

export default TablePaginator;
