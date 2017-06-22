import query from 'url-query';

export const getSearch = () => {
  const search = window.location.search;
  if (search) return query(search);
  return {};
};

export const x = 1;
