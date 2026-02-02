import { json } from '@sveltejs/kit';

export function GET() {
      return json({
            alive: true
      }, {
            status: 200
      });
}