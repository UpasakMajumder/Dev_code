// @flow
/* helpers */
import { consoleException } from 'app.helpers/io';
/* 3rd part libraries */
import Pagination from '../paginator';

class TablePaginator {
  constructor(tableWrapper: HTMLElement) {
    const startPageNumber: number = 1;
    const step: number = 1;
    const paginatorPageFromClass: string = 'js-table-paginator-from';
    const paginatorPageToClass: string = 'js-table-paginator-to';
    const paginatorWrapperClass: string = 'js-table-paginator-wrapper';
    const paginatorFromPage: ?HTMLElement = tableWrapper.querySelector(`.${paginatorPageFromClass}`);
    const paginatorToPage: ?HTMLElement = tableWrapper.querySelector(`.${paginatorPageToClass}`);
    const paginatorWrapper: ?HTMLElement = tableWrapper.querySelector(`.${paginatorWrapperClass}`);

    if (TablePaginator.handleNodeSearchException(paginatorFromPage, paginatorPageFromClass)
      || TablePaginator.handleNodeSearchException(paginatorToPage, paginatorPageToClass)
      || TablePaginator.handleNodeSearchException(paginatorWrapper, paginatorWrapperClass)) return;

    // Flow doesn't consider handleNodeSearchException
    // $FlowIgnore
    const { pages: paginatorPagesNumber, rowsOnPage: paginatorRowsToShow } = paginatorWrapper.dataset;

    const init = () => {
      const paginator: {} = new Pagination(paginatorWrapper, {
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

  static handleNodeSearchException(element: ?HTMLElement, selector: string): boolean {
    if (!element || !(element instanceof HTMLElement)) {
      consoleException(`No element with .${selector} selector`);
      return true;
    }
    return false;
  }

  static callback(prevPage: number, currPage: number, wrapper: HTMLElement, to: HTMLElement, from: HTMLElement, rowsOnPage: number): void {
    const rowActiveClass: string = 'active';
    let toNumber: number = 0;

    const findRows = (num: number): ?HTMLElement[] => {
      const rows = wrapper.querySelectorAll(`tr[data-page="${num}"]`);
      if (!rows.length) {
        consoleException(`No element with tr[data-page="${num}"] selector`);
        return null;
      }
      return Array.from(rows);
    };

    const unstyleActiveRows = (rows): void => {
      rows.forEach(row => row.classList.remove(rowActiveClass));
    };

    const styleActiveRows = (rows): void => {
      rows.forEach(row => row.classList.add(rowActiveClass));
    };

    const prevRows: ?HTMLElement[] = findRows(prevPage);
    const nextRows: ?HTMLElement[] = findRows(currPage);

    if (!prevRows || !nextRows) {
      consoleException('No found rows');
      return;
    }

    const fromNumber: number = (((currPage - 1) * rowsOnPage) + 1);

    if (nextRows.length < rowsOnPage) {
      toNumber = ((currPage - 1) * rowsOnPage) + nextRows.length;
    } else {
      toNumber = currPage * rowsOnPage;
    }

    unstyleActiveRows(prevRows);
    styleActiveRows(nextRows);

    from.innerHTML = fromNumber.toString();
    to.innerHTML = toNumber.toString();
  }
}

export default TablePaginator;
