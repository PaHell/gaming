<script lang="ts">
	import data from './data.js';
	import * as Sidebar from '$lib/shadcn/components/ui/sidebar/index.js';
	import AppSidebar from '$lib/shadcn/components/app-sidebar.svelte';
	import SiteHeader from '$lib/shadcn/components/site-header.svelte';
	import SectionCards from '$lib/shadcn/components/section-cards.svelte';
	import ChartAreaInteractive from '$lib/shadcn/components/chart-area-interactive.svelte';
	import DataTable from '$lib/shadcn/components/data-table.svelte';
	import StockChart from '@/custom/stock-chart.svelte';
</script>

<SiteHeader />
<div class="flex flex-1 flex-col">
	<div class="@container/main flex flex-1 flex-col gap-2">
		<div class="flex flex-col gap-4 py-4 md:gap-6 md:py-6">
			<SectionCards />
			<div class="grid grid-cols-2 gap-4 px-4 lg:gap-6 lg:px-6">
				<StockChart name="AAPL" />
			</div>
			<div class="px-4 lg:px-6">
				<ChartAreaInteractive />
			</div>
			<DataTable {data} />
		</div>
	</div>
</div>
