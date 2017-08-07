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

/**
 * @param func   compare func
 * @param list1
 * @param list2
 * @returns {Array}
 */

export const compareArrays = (func, list1, list2) => {
  const array = [];
  list1.forEach((item, index) => {
    func(item, list2[index], array);
  });
  return array;
};
