<script lang="ts">
	import * as Sidebar from '$lib/shadcn/components/ui/sidebar/index.js';
	import AppSidebar from '$lib/shadcn/components/app-sidebar.svelte';
	import { Button } from '$lib/shadcn/components/ui/button/index.js';
	import { Input } from '$lib/shadcn/components/ui/input/index.js';
	import type { LayoutProps } from './$types';
	import * as InputGroup from '$lib/shadcn/components/ui/input-group/index.js';
	import * as DropdownMenu from '$lib/shadcn/components/ui/dropdown-menu/index.js';
	import * as Tooltip from '$lib/shadcn/components/ui/tooltip/index.js';
	import { Separator } from '$lib/shadcn/components/ui/separator/index.js';
	import SearchIcon from '@lucide/svelte/icons/search';
	import CalendarIcon from '@lucide/svelte/icons/calendar';
	import { type DateValue, DateFormatter, getLocalTimeZone } from '@internationalized/date';
	import { cn } from '$lib/shadcn/utils.js';
	import { Calendar } from '$lib/shadcn/components/ui/calendar/index.js';
	import * as Popover from '$lib/shadcn/components/ui/popover/index.js';
	import type { FormEventHandler } from 'svelte/elements';
	import { StockSymbolClient, type SymbolSearchMatch } from '@/clients';
	import { debounce } from '@/custom/debounce';
	import { env } from '$env/dynamic/public';
	import * as Command from '$lib/shadcn/components/ui/command/index.js';

	let { children, data }: LayoutProps = $props();

	let searchOpen = $state(false);
	let searchValue = $state('');
	let searchTrigger: HTMLDivElement | null = $state(null);
	let searchContent: HTMLDivElement | null = $state(null);
	let searchResults = $state<SymbolSearchMatch[]>([]);

	let debouncedOnSearch = debounce(onSearch, 100);

	async function onSearch() {
		try {
			console.log('Searching for:', searchValue);
			const client = new StockSymbolClient(env.PUBLIC_BACKEND_URL_CLIENT);
			searchResults = await client.search(searchValue);
			console.log('Search results:', searchResults);
		} catch (error) {
			console.error('Error searching symbols:', error);
		}
	}
</script>

<Sidebar.Provider
	style="--sidebar-width: calc(var(--spacing) * 72); --header-height: calc(var(--spacing) * 12);"
>
	<AppSidebar variant="inset" />
	<Sidebar.Inset>
		<header
			class="flex h-(--header-height) shrink-0 items-center gap-2 border-b transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-(--header-height)"
		>
			<div class="flex w-full items-center gap-1 px-4 lg:gap-2 lg:px-6">
				<Sidebar.Trigger class="-ml-1" />
				<Separator orientation="vertical" class="mx-2 data-[orientation=vertical]:h-4" />
				<h1 class="pe-2 text-base font-medium">Dashboard</h1>
				<div class="ml-auto flex items-center gap-2">
					<div class="flex flex-col">
						<InputGroup.Root bind:ref={searchTrigger}>
							<InputGroup.Input
								type="search"
								bind:value={searchValue}
								placeholder="Search..."
								oninput={debouncedOnSearch}
								onfocus={() => (searchOpen = true)}
								onblur={() => (searchOpen = false)}
							/>
							<InputGroup.Addon>
								<SearchIcon />
							</InputGroup.Addon>
							{#if searchResults.length > 0}
								<InputGroup.Addon align="inline-end"
									>{searchResults.length} results</InputGroup.Addon
								>
							{/if}
						</InputGroup.Root>
						<div bind:this={searchContent} class="relative h-0" class:hidden={!searchOpen}>
							<div
								class="mt-px origin-(--bits-popover-content-transform-origin) rounded-md border bg-popover p-1 text-popover-foreground shadow-md outline-hidden data-[side=bottom]:slide-in-from-top-2 data-[side=left]:slide-in-from-right-2 data-[side=right]:slide-in-from-left-2 data-[side=top]:slide-in-from-bottom-2 data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=closed]:zoom-out-95 data-[state=open]:animate-in data-[state=open]:fade-in-0 data-[state=open]:zoom-in-95"
							>
								{#each searchResults as result}
									<a
										href={`/app/stocks/${result.symbol}`}
										class="relative flex cursor-default items-center gap-2 rounded-sm px-2 py-1.5 text-sm outline-hidden select-none data-highlighted:bg-accent data-highlighted:text-accent-foreground data-[disabled]:pointer-events-none data-[disabled]:opacity-50 data-[inset]:pl-8 data-[variant=destructive]:text-destructive data-[variant=destructive]:data-highlighted:bg-destructive/10 data-[variant=destructive]:data-highlighted:text-destructive dark:data-[variant=destructive]:data-highlighted:bg-destructive/20 [&_svg]:pointer-events-none [&_svg]:shrink-0 [&_svg:not([class*='size-'])]:size-4 [&_svg:not([class*='text-'])]:text-muted-foreground data-[variant=destructive]:*:[svg]:!text-destructive"
									>
										{result.name}
									</a>
								{/each}
								{#if searchResults.length === 0}
									<div class="px-2 py-1 text-sm text-muted-foreground">No results found.</div>
								{/if}
							</div>
						</div>
					</div>
					<Button
						href={data.npm_package_github_url}
						variant="ghost"
						size="sm"
						class="hidden sm:flex dark:text-foreground"
						target="_blank"
						rel="noopener noreferrer"
					>
						GitHub
					</Button>
				</div>
			</div>
		</header>
		{@render children()}
	</Sidebar.Inset>
</Sidebar.Provider>
