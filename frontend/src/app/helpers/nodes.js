// @flow
/* helpers */
import { consoleException } from 'app.helpers/io';

export default function findParentBySelector(elm: Node, selector: string): ?Node {
  const all: Node[] = Array.from(document.querySelectorAll(selector));
  let curr: ?Node = elm.parentNode;

  if (!curr) {
    consoleException('No parent node', elm);
    return null;
  }

  while (curr && !all.includes(curr)) {
    if (!curr.parentNode) {
      consoleException('No parent node', elm);
      return curr;
    }

    curr = curr.parentNode;
  }
  return curr;
}
