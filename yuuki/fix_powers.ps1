$files = Get-ChildItem -Path "e:\文件\杀戮尖塔mod\杀戮尖塔2自制\mod\雪雪mod\yuuki\Scripts\Cards" -Recurse -Filter "*.cs" | Where-Object { $_.Name -notmatch "Var\.cs$" -and $_.Name -ne "YukiCardModel.cs" }

foreach ($f in $files) {
    $text = Get-Content $f.FullName -Raw -Encoding UTF8
    $matches = [regex]::Matches($text, 'PowerCmd\.Apply<([A-Za-z0-9_]+)>')
    if ($matches.Count -gt 0) {
        $powers = @()
        foreach ($m in $matches) {
            $powers += $m.Groups[1].Value
        }
        $powers = $powers | Select-Object -Unique
        
        $changed = $false
        foreach ($p in $powers) {
            if ($text -notmatch "PowerVar<$p>") {
                $changed = $true
                
                # Convert Array.Empty to [] so the next replace works
                $text = $text -replace 'CanonicalVars\s*=>\s*Array\.Empty<DynamicVar>\(\)', "CanonicalVars => []"
                
                if ($text -match 'CanonicalVars\s*=>\s*\[([^\]]*)\]') {
                    $text = $text -replace '(CanonicalVars\s*=>\s*\[)([^\]]*)(\])', "${1}${2}, new PowerVar<$p>(1m)${3}"
                    $text = $text -replace '\[\s*,', '['
                } else {
                    $className = $f.BaseName
                    $insertStr = "
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<$p>(1m)];
"
                    if ($text -match "public $className\(\)") {
                        $text = $text -replace "public $className\(\)", "$insertStr	public $className()"
                    } elseif ($text -match "protected override async Task OnPlay") {
                        $text = $text -replace "protected override async Task OnPlay", "$insertStr	protected override async Task OnPlay"
                    }
                }
            }
        }
        if ($changed) {
            if ($text -notmatch "using MegaCrit\.Sts2\.Core\.Localization\.DynamicVars;") {
                $text = "using MegaCrit.Sts2.Core.Localization.DynamicVars;
" + $text
            }
            Set-Content $f.FullName $text -Encoding UTF8
            Write-Host "Fixed: $($f.Name)"
        }
    }
}
