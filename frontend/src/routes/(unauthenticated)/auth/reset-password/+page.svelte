<script lang="ts">
	import { env } from '$env/dynamic/public';
	import LoginForm from '$lib/shadcn/components/login-form.svelte';
	import GalleryVerticalEndIcon from '@lucide/svelte/icons/gallery-vertical-end';
	import { Button } from '$lib/shadcn/components/ui/button/index.js';
	import * as Card from '$lib/shadcn/components/ui/card/index.js';
	import {
		FieldGroup,
		Field,
		FieldLabel,
		FieldDescription,
		FieldSeparator
	} from '$lib/shadcn/components/ui/field/index.js';
	import { Input } from '$lib/shadcn/components/ui/input/index.js';
	import { cn } from '$lib/shadcn/utils.js';
	import type { HTMLAttributes } from 'svelte/elements';
</script>

<Card.Header class="text-center">
	<Card.Title class="text-xl">Reset password</Card.Title>
	<Card.Description>and sign in</Card.Description>
</Card.Header>
<Card.Content>
	<form>
		<FieldGroup>
			<FieldLabel>Email</FieldLabel>
			<p class="-mt-4">me@example.com</p>
			<Field>
				<FieldLabel for="password">Password</FieldLabel>
				<Input id="password" type="password" required />
			</Field>
			<Field>
				<FieldLabel for="password">Repeat password</FieldLabel>
				<Input id="password-2" type="password" required />
			</Field>
			<Field>
				<Button type="submit">Update Password</Button>
			</Field>
		</FieldGroup>
	</form>
</Card.Content>
