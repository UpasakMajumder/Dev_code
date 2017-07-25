class Redirection {
  constructor(link) {
    const { url, blank } = link.dataset;

    link.addEventListener('click', (event) => {
      if (event.target.classList.contains('js-redirection-ignore')) return;

      if (blank === 'true') {
        window.open(url, '_blank');
      } else {
        document.location = url;
      }
    });
  }
}

export default Redirection;
