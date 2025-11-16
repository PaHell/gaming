export const load: import('./$types').LayoutLoad = (({ params }) => {
      return {
            stockId: params.stock,
      }
});