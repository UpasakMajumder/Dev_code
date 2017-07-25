// @flow
export const paginationFilter = (list: {}[], currentPage: number, pagesNumber: number): {}[] => {
  const minPage: number = pagesNumber * currentPage;
  const maxPage: number = (pagesNumber * (currentPage + 1)) - 1;

  return list.filter((page, index) => index >= minPage && index <= maxPage);
};

export const bla = 1;
