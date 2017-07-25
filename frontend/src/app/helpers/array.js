export const paginationFilter = (list, currentPage, filterNumber) => {
  const minPage = filterNumber * currentPage;
  const maxPage = (filterNumber * (currentPage + 1)) - 1;

  return list.filter((page, index) => index >= minPage && index <= maxPage);
};

export const bla = 1;
