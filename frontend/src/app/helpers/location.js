import query from 'url-query';

export const getSearchObj = () => {
  const search = window.location.search;
  if (search) return query(search);
  return {};
};

export const createSearchStr = (obj) => {
  let search = '?';
  const keys = Object.keys(obj);
  keys.forEach((key, index) => {
    if (index) {
      search += `&${key}=${obj[key]}`;
    } else {
      search += `${key}=${obj[key]}`;
    }
  });
  return search;
};

export const createNewUrl = (data, loc) => {
  const { protocol: locationProtocol,
          host: locationHost,
          pathname: locationPathname,
          search: locationSearch } = loc;

  const { protocol: dataProtocol,
          host: dataHost,
          pathname: dataPathname,
          search: dataSearch } = data;
  let newUrl = '';

  // Set protocol
  if (dataProtocol) {
    newUrl += `${dataProtocol}//`;
  } else {
    newUrl += `${locationProtocol}//`;
  }

  // Set host
  if (dataHost) {
    newUrl += dataHost;
  } else {
    newUrl += locationHost;
  }

  // Set pathname
  if (dataPathname) {
    newUrl += dataPathname;
  } else {
    newUrl += locationPathname;
  }

  // Set search
  if (dataSearch) {
    const locationSearchObj = locationSearch ? query(locationSearch) : {};
    const { props, method } = dataSearch; // methods: set / clear

    if (method === 'set') {
      const keys = Object.keys(props);
      const values = Object.values(props);
      keys.forEach((key, index) => locationSearchObj[key] = values[index]);
      newUrl += createSearchStr(locationSearchObj);
    } else {
      newUrl += '';
    }
  } else {
    newUrl += locationSearch;
  }

  return newUrl;
};
