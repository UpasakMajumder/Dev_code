// @flow
// $FlowIgnore
import query from 'url-query';
/* constants */
import { DOMAIN } from 'app.consts';

export const getSecondLevelDomain = () => {
  const { domain } = document;
  if (!domain) return null;
  const array = domain.split('.');
  const index = array.indexOf(DOMAIN);
  if (index === -1) return false;
  return [array[index - 1], array[index]].join('.');
};

export const getSearchObj = (): { orderID?: string } => {
  const search: ?string = window.location.search;
  if (search) return query(search);
  return {};
};

export const createSearchStr = (obj: {}): string => {
  let search = '?';
  const keys: number[] = Object.keys(obj);
  keys.forEach((key, index) => {
    if (index) {
      search += `&${key}=${obj[key]}`;
    } else {
      search += `${key}=${obj[key]}`;
    }
  });
  return search;
};

export const createNewUrl = (data: {
  protocol?: string,
  host?: string,
  pathname?: ?string,
  search?: { props: {}, method: ?string }
}) => {
  const { protocol: dataProtocol,
    host: dataHost,
    pathname: dataPathname,
    search: dataSearch } = data;

  const { protocol: locationProtocol,
          host: locationHost,
          pathname: locationPathname,
          search: locationSearch } = location;

  let newUrl: string = '';

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
    newUrl += locationSearch || '';
  }

  return newUrl;
};
