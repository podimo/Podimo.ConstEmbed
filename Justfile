# https://github.com/casey/just

# Print generated types and their constants.
print-types:
    dotnet run --nologo --verbosity=quiet --project examples/Podimo.ExampleConsoleApp

# Print generated code.
print-code:
    dotnet run --nologo --verbosity=quiet --project examples/Podimo.ExampleCodeGeneration

# Run the tests.
test:
    dotnet test --nologo --verbosity=quiet

# List the contents of the generated nupkg. N.B.: Requires bash.
inspect:
    #!/usr/bin/env bash
    set -euo pipefail
    my_tmp_dir=$(mktemp -d 2>/dev/null || mktemp -d -t 'my_tmp_dir')  # 1. Create temporary directory
    dotnet pack --nologo --verbosity=quiet --output "${my_tmp_dir}"   # 2. Put packages into directory
    for x in "${my_tmp_dir}"/*.nupkg; do                              # 3. Iterate through all packages
      mv -- "$x" "$x.zip"                                             # 4. Set zip extension on packages
      unzip -l "$x.zip"                                               # 5. Use unzip to list the files
    done
    rm -rf "${my_tmp_dir}"                                            # 6. Remove the directory
