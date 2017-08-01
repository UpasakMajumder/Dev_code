// @flow
export const paginationFilter = (list: {}[], currentPage: number, pagesNumber: number): {}[] => {
  const minPage: number = pagesNumber * currentPage;
  const maxPage: number = (pagesNumber * (currentPage + 1)) - 1;

  return list.filter((page, index) => index >= minPage && index <= maxPage);
};

/**
 * @param number
 * @param arr
 * @returns {Array}
 */

export const filterByLessNumber = (number, arr) => {
  if (!arr) return [];
  return arr.filter((item, index) => index < number);
};
