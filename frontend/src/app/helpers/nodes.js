export default function findParentBySelector(elm, selector) {
  const all = Array.from(document.querySelectorAll(selector));
  let cur = elm.parentNode;

  while(cur && !all.includes(cur)) {
    cur = cur.parentNode;
  }
  return cur;
}
