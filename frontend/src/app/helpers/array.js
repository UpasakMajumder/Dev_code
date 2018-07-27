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

export const filterByLessNumber = (number: number, arr: number[]): number[] => {
  if (!arr) return [];
  return arr.filter((item: number, index: number): boolean => index < number);
};

/**
 * @param func   compare func
 * @param list1
 * @param list2
 * @returns {Array}
 */

export const compareArrays = (func: Function, list1: any[], list2: any[]) => {
  const array = [];
  list1.forEach((item, index) => {
    func(item, list2[index], array);
  });
  return array;
};

export const sortObjs = (array: any[], name: string, sortOrderAsc: boolean, level: number) => {
  return [...array].sort((obj1, obj2) => {
    const prop1 = level ? obj1[level][name] : obj1[name];
    const prop2 = level ? obj2[level][name] : obj2[name];


    if (prop1 === null) {
      return Number.NEGATIVE_INFINITY;
    }
    if (prop2 === null) {
      return Number.POSITIVE_INFINITY;
    }

    const name1 = prop1.toUpperCase();
    const name2 = prop2.toUpperCase();

    if (sortOrderAsc) {
      if (name1 < name2) return 1;
      return -1;
    }

    if (name1 > name2) return 1;
    return -1;
  });
};
