// @flow

/* helpers */
import { consoleException } from 'app.helpers/io';

class Redirection {
  constructor(link: HTMLElement) {
    const { url, blank }: { url: string, blank: string } = link.dataset;

    if (!url) {
      consoleException('Check link element, it must have data-url', link);
      return;
    }

    link.addEventListener('click', (event: Event): void => {
      if (!(event.target instanceof HTMLElement)) return;
      if (event.target.classList.contains('js-redirection-ignore')) return;

      if (blank === 'true') {
        window.open(url, '_blank');
      } else {
        document.location.replace(url);
      }
    });
  }
}

export default Redirection;
