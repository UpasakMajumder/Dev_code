export const getOffsetTop = (el) => {
  const currentScrollPosition =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;

  return el.getBoundingClientRect().top + currentScrollPosition;
};

export const scrollTo = (selector) => {
  const gutter = 20;
  const target = document.querySelector(selector);
  if (!target) return false;
  const targetOffsetTop = getOffsetTop(target);
  window.scroll({
    behavior: 'smooth',
    top: targetOffsetTop - gutter
  });
  return true;
};
